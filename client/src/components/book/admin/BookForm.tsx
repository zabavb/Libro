import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { icons } from '@/lib/icons'
import "@/assets/styles/components/book/book-form.css"
import { getUserFromStorage } from '@/utils/storage';
import { useNavigate } from 'react-router-dom';
import { Book, User } from '@/types';
import noImageUrl from '@/assets/noImage.svg'
import { BookFormData, bookValidationSchema } from '@/utils/bookValidationSchema';
import { dateToString } from '@/api/adapters/commonAdapters';
import { CoverType } from '@/types/subTypes/book/CoverType';
import { Language } from '@/types/subTypes/book/Language';
import { coverNumberToEnum, languageNumberToEnum } from '@/api/adapters/bookAdapter';
interface BookFormProps {
    existingBook?: Book;
    onAddBook: (book: BookFormData) => Promise<void>;
    onEditBook: (
        updatedBook: BookFormData,
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
            year: dateToString(new Date(new Date().getFullYear())),
            cover: CoverType.DUST_JACKET,
            price: 0,
            language: Language.ENGLISH,
            quantity: 0,
            authorId: '',
            publisherId: '',
            categoryId: '',
            description: ''
        },
    });

    const [imagePreview, setImagePreview] = useState<string | null>(null);
    const languageValue = watch("language");
    const coverValue = watch("cover");
    const navigate = useNavigate();
    const [localEdit, setLocalEdit] = useState<boolean>(isEdit);

    useEffect(() => {
        if (existingBook === undefined)
            setLocalEdit(true)
        else {
            setLocalEdit(isEdit)
        }
    }, [isEdit, existingBook])

    const loggedUser: User | null = getUserFromStorage();



    useEffect(() => {
        if (existingBook) {
            setValue('title', existingBook.title ?? undefined);
            setValue('year', dateToString(existingBook.year));
            setValue('cover', coverNumberToEnum(existingBook.cover));
            setValue('price', existingBook.price);
            setValue('language', languageNumberToEnum(existingBook.language));
            setValue('quantity', existingBook.quantity);
            setValue('authorId', existingBook.authorId);
            setValue('categoryId', existingBook.categoryId);
            setValue('publisherId', existingBook.publisherId);
            setValue('description', existingBook.description ?? '');
        }
    }, [existingBook, setValue]);

    const onSubmit = (data: BookFormData) => {
        console.log(data)
        // const formData = new FormData();
        // formData.append(
        //     'id',
        //     existingBook?.bookId ?? '00000000-0000-0000-0000-000000000000',
        // )
        // formData.append('year', data.year);
        // formData.append('cover', data.cover);
        // formData.append('price', data.price.toString());
        // formData.append('language', data.language);
        // formData.append('quantity', data.quantity.toString());
        // formData.append('authorId', data.authorId);
        // formData.append('publisherId', data.publisherId);
        // formData.append('categoryId', data.categoryId);
        // formData.append('description', data.description);
        // formData.append('image', data.image ?? '');
        // if (existingBook)
        //     formData.append('imageUrl', existingBook.imageUrl ?? '');

        if (existingBook) onEditBook(data);
        else onAddBook(data);
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
                        {!imagePreview && <img className='w-[260px] h-[390px]' src={noImageUrl} />}
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
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Author</label>
                                <input {...register('authorId')}
                                    className='input-field'
                                    placeholder='Author'
                                    disabled={!localEdit} />
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
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Publisher</label>
                                <input {...register('publisherId')}
                                    className='input-field'
                                    placeholder='Publisher'
                                    disabled={!localEdit} />
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
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Category</label>
                                <input {...register('categoryId')}
                                    className='input-field'
                                    placeholder='Category'
                                    disabled={!localEdit} />
                                <p>{errors.categoryId?.message}</p>
                            </div>
                            <div className='input-row flex-1'>
                                <label className='text-sm'>Subcategory</label>
                                <input
                                    // {...register('categoryId')}
                                    className='input-field'
                                    placeholder='Subcategory'
                                    disabled={true} />
                                <p>{errors.categoryId?.message}</p>
                            </div>
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

