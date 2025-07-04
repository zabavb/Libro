import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { icons } from '@/lib/icons'
import "@/assets/styles/components/book/book-form.css"
import { getUserFromStorage } from '@/utils/storage';
import { useNavigate } from 'react-router-dom';
import { User } from '@/types';
import noImageUrl from '@/assets/noImage.svg'
import { BookFormData, bookValidationSchema } from '@/utils/bookValidationSchema';
import { CoverType } from '@/types/subTypes/book/CoverType';
import { Language } from '@/types/subTypes/book/Language';
// import { coverNumberToEnum, languageNumberToEnum } from '@/api/adapters/bookAdapter';
import BookFormAuthorSearch from '../BookFormAuthorSearch';
import BookFormPublisherSearch from '../BookFormPublisherSearch';
import BookFormCategorySearch from '../BookFormCategorySearch';
import BookFormSubCategorySearch from '../BookFormSubcategorySearch';
import BookSubcategoryList from '../BookSubcategoryList';
import { BookDetails } from '@/types/types/book/BookDetails';
interface BookFormProps {
    existingBook?: BookDetails;
    onAddBook: (book: FormData) => Promise<void>;
    onEditBook: (
        updatedBook: FormData,
    ) => Promise<void>;
    loading: boolean;
    onIsEdit: (isEdit: boolean) => void;
    isEdit: boolean;
}

const BookForm: React.FC<BookFormProps> = ({
    existingBook,
    onAddBook,
    onEditBook,
    loading,
    isEdit,
    onIsEdit
}) => {
    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors },
        watch,
    } = useForm<BookFormData>({
        resolver: zodResolver(bookValidationSchema),
        defaultValues: {
            title: '',
            year: new Date(new Date().getFullYear(), 0, 1).toISOString(),
            cover: CoverType.DUSTJACKET,
            price: 0,
            language: Language.ENGLISH,
            quantity: 0,
            authorId: '',
            publisherId: '',
            categoryId: '',
            discountId: undefined, 
            subcategoryIds: [],   
            description: '',
            image: undefined,
            audio: undefined,
            PDF: undefined,
        }
    });

    const [imagePreview, setImagePreview] = useState<string | null>(null);
    const languageValue = watch("language");
    const coverValue = watch("cover");
    const navigate = useNavigate();
    const [localEdit, setLocalEdit] = useState<boolean>(isEdit);
    const [selectedAuthor, setSelectedAuthor] = useState<string>("");
    const [selectedPublisher, setSelectedPublisher] = useState<string>("");
    const [selectedCategory, setSelectedCategory] = useState<string>("");
    const [selectedSubCategoriesDisplay, setSelectedSubCategoriesDisplay] = useState<Record<string,string>>({});
    useEffect(() => {
        if (existingBook === undefined)
            setLocalEdit(true)
        else {
            setLocalEdit(isEdit)
        }
    }, [isEdit, existingBook])

    const loggedUser: User | null = getUserFromStorage();

    const addSubCategory = (subcategoryId: string, subcategoryName: string) => {
        setSelectedSubCategoriesDisplay(prev => ({
            ...(prev || {}),
            [subcategoryId]: subcategoryName,
        }));
    };

    const removeSubCategory = (subcategoryId: string) => {
        setSelectedSubCategoriesDisplay(prev => {
            if (!prev) return prev;
            // eslint-disable-next-line @typescript-eslint/no-unused-vars
            const { [subcategoryId]: _, ...rest } = prev;
            return rest;
        });
    };

    useEffect(() => {
        console.log("Data:", existingBook);
        if (existingBook) {
            setValue('title', existingBook.title ?? undefined);
            setValue('year', new Date(existingBook.year).getFullYear().toString());
            setValue('cover', /* coverNumberToEnum( */existingBook.cover/* ) */);
            setValue('price', existingBook.price.toFixed(2) as unknown as number);
            setValue('language', /* languageNumberToEnum( */existingBook.language/* ) */);
            setValue('quantity', existingBook.quantity);
            setValue('authorId', existingBook.authorName);
            setValue('categoryId', existingBook.categoryName ?? '');
            setValue('publisherId', existingBook.publisherName ?? '');
            setValue('description', existingBook.description ?? '');
            /* setValue('discountId', existingBook.dis ?? undefined); */
            setValue('subcategoryIds', existingBook.subcategories ?? []);
        }
    }, [existingBook, setValue]);

    const onSubmit = (data: BookFormData) => {
        const formData = new FormData();

        formData.append("Title", data.title);
        formData.append("AuthorId", selectedAuthor);
        formData.append("PublisherId", selectedPublisher);
        formData.append("CategoryId", selectedCategory);
        formData.append('Price', data.price.toString());
        formData.append("Language", data.language);
        formData.append("Year", new Date(Number(data.year), 1, 1).toISOString());
        formData.append("Description", data.description || "");
        formData.append("Cover", data.cover);
        formData.append("Quantity", data.quantity.toString());

        if (data.discountId) {
            formData.append("DiscountId", data.discountId);
        }

        if (data.image instanceof File) {
            formData.append("Image", data.image);
        }

        if (data.audio instanceof File) {
            formData.append("Audio", data.audio);
        }

        if (data.PDF instanceof File) {
            formData.append("PDF", data.PDF);
        }

        if (existingBook){
            formData.append("ImageUrl", existingBook.imageUrl || "");
        }
        
        if (selectedSubCategoriesDisplay && Object.entries(selectedSubCategoriesDisplay).length > 0) {
            Object.entries(selectedSubCategoriesDisplay).forEach(([key]) => {
                formData.append("SubcategoryIds", key);
            });
        }

        if (existingBook) {
            formData.append("BookId", existingBook.bookId);
            onEditBook(formData);
        } else {
            onAddBook(formData);
        }
    };

    const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const file = event.target.files?.[0];
        if (file) {
            const imageUrl = URL.createObjectURL(file);
            setImagePreview(imageUrl);
            setValue('image', file);
        }
    };

    return (
        <div>
            <header className='header-container'>
                <div className='flex gap-[60px] items-center'>
                    <div className='flex gap-5 items-center'>
                        <button className='form-back' onClick={() => navigate('/admin/booksRelated/books')}>
                            <img src={icons.oArrowLeft} />
                        </button>
                        <h1 className='text-2xl font-semibold'>Books</h1>
                    </div>
                    {existingBook &&
                        (
                            <button className={`form-edit-btn ${localEdit === true && "edit-active"}`} onClick={() => { onIsEdit(!isEdit) }}>
                                Edit
                            </button>
                        )
                    }

                </div>
                <div className="profile-icon">
                    <div className="icon-container-pfp">
                        <img src={loggedUser?.imageUrl ? loggedUser.imageUrl : icons.bUser} className="panel-icon" />
                    </div>
                    <p className="profile-name">{loggedUser?.firstName ?? "Unknown User"} {loggedUser?.lastName}</p>
                </div>
            </header>
            <main className='flex px-[55px] py-4 gap-4'>
                <div className='flex flex-col'>
                    {/* <img className='w-[260px] h-[390px]'
                        src={`${existingBook ? `https://picsum.photos/seed/${existingBook?.bookId}/260/390` : noImageUrl}`} /> */}
                    <label
                        htmlFor='imageUpload'
                        className='flex items-center justify-center cursor-pointer
           overflow-hidden bg-contain bg-no-repeat bg-center w-[260px] h-[390px]'
                        style={{ backgroundImage: imagePreview ? `url(${imagePreview})` : existingBook?.imageUrl ? `url(${existingBook.imageUrl})` : "none", }}
                    >
                        {!existingBook?.imageUrl && (!imagePreview && <img className='w-[260px] h-[390px]' src={noImageUrl} />)}
                    </label>
                    <p>{errors.image?.message}</p>

                </div>
                <div className='flex flex-col gap-[33px] w-full'>
                    <form onSubmit={handleSubmit(onSubmit)} className='flex flex-col gap-4 w-full'>
                        <div className='flex gap-2'>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Title</label>
                                <input {...register('title')}
                                    className='input-field'
                                    placeholder='Title'
                                    disabled={!localEdit} />
                                <p>{errors.title?.message}</p>
                            </div>
                            {/* To be replaced */}
                            {/* <div className='input-row flex-1'>
                                <label className='text-sm'>Author</label>
                                <input {...register('authorId')}
                                    className='input-field'
                                    placeholder='Author'
                                    disabled={!localEdit} />
                            </div> */}
                            <input {...register('authorId')}
                                value={selectedAuthor}
                                type='hidden'
                            />
                            <div className='flex flex-col flex-1'>
                                <BookFormAuthorSearch
                                    existingAuthor={existingBook?.authorName}
                                    onSelect={setSelectedAuthor}
                                    isEnabled={!localEdit} />
                                <p>{errors.authorId?.message}</p>
                            </div>
                        </div>
                        <div className='flex gap-2'>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Language</label>
                                <select
                                    className={`input-select ${languageValue.toLowerCase()}`}
                                    {...register("language")}>
                                    {Object.entries(Language).map(([key, value]) => (
                                        <option
                                            className="select-option"
                                            key={key}
                                            value={value}>
                                            {value}
                                        </option>
                                    ))}
                                </select>
                                <p>{errors.language?.message}</p>
                            </div>
                            {/* <div className='input-row flex-1'>
                                <label className='text-sm'>Publisher</label>
                                <input {...register('publisherId')}
                                    className='input-field'
                                    placeholder='Publisher'
                                    disabled={!localEdit} />
                                
                            </div> */}
                            <input {...register('publisherId')}
                                value={selectedPublisher}
                                type='hidden'
                            />
                            <div className='flex flex-col flex-1'>
                                <BookFormPublisherSearch
                                    existingPublisher={existingBook?.publisherName}
                                    onSelect={setSelectedPublisher}
                                    isEnabled={!localEdit} />
                                <p>{errors.publisherId?.message}</p>
                            </div>
                        </div>
                        <div className='flex gap-2'>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Release year</label>
                                <input {...register('year')}
                                    className='input-field'
                                    placeholder='Year'
                                    type='number'
                                    disabled={!localEdit} />
                                <p>{errors.year?.message}</p>
                            </div>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Cover</label>
                                <select
                                    className={`input-select ${coverValue.toLowerCase()}`}
                                    {...register("cover")}>
                                    {Object.entries(CoverType).map(([key, value]) => (
                                        <option
                                            className="select-option"
                                            key={key}
                                            value={value}>
                                            {value}
                                        </option>
                                    ))}
                                </select>
                                <p>{errors.cover?.message}</p>
                            </div>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Price</label>
                                <input {...register('price')}
                                    className='input-field'
                                    placeholder='Price'
                                    type='number'
                                    disabled={!localEdit} />
                                <p>{errors.price?.message}</p>
                            </div>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Quantity</label>
                                <input {...register('quantity', { valueAsNumber: true })}
                                    className='input-field'
                                    placeholder='Quantity'
                                    type='number'
                                    disabled={!localEdit} />
                                <p>{errors.quantity?.message}</p>
                            </div>
                        </div>
                        <div className='flex gap-2'>
                            <div className='flex flex-col flex-1'>
                                <BookFormCategorySearch
                                    existingCategory={existingBook?.categoryName}
                                    onSelect={setSelectedCategory}
                                    isEnabled={!localEdit} />
                                <p>{errors.authorId?.message}</p>
                            </div>
                            <div className='flex flex-col flex-1'>
                                <BookFormSubCategorySearch
                                    existingSubCategories={existingBook?.subcategories ?? []}
                                    onSelect={addSubCategory}
                                    isEnabled={!localEdit} />
                                <p>{errors.authorId?.message}</p>
                            </div>
                        </div>
                        <div className='flex gap-2'>
                            <BookSubcategoryList
                                RemoveSubcategory={removeSubCategory}
                                Subcategories={selectedSubCategoriesDisplay ?? {}}
                                isEnabled={localEdit}
                            />
                        </div>
                        <div className='flex gap-2'>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Description</label>
                                <textarea
                                    rows={20}
                                    className='input-field resize-none'
                                    {...register('description')}
                                    placeholder='Description'
                                    disabled={!localEdit}
                                />
                                <p>{errors.description?.message}</p>
                            </div>
                        </div>
                        <button type='submit' disabled={loading} className='form-edit-btn fixed bottom-6 right-14'>
                            {existingBook ? 'Save changes' : 'Add Book'}
                        </button>
                        <input
                            id='imageUpload'
                            type='file'
                            accept='image/*'
                            style={{ display: 'none' }}
                            onChange={handleImageChange}
                        />
                    </form>
                </div>
            </main>
        </div>
    );
};

export default BookForm;

