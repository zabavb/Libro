import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { icons } from '@/lib/icons'
import "@/assets/styles/components/user/user-form.css"
import { getUserFromStorage } from '@/utils/storage';
import { useNavigate } from 'react-router-dom';
import { Author, User } from '@/types';
import { AuthorFormData, authorSchema } from '@/utils';
import { dateToString } from '@/api/adapters/commonAdapters';
import noImageUrl from '@/assets/noImage.svg'
interface AuthorFormProps {
  existingAuthor?: Author;
  onAddAuthor: (author: FormData) => Promise<void>;
  onEditAuthor: (
    updatedAuthor: FormData,
  ) => Promise<void>;
  loading: boolean;
  onIsEdit: (isEdit: boolean) => void;
  isEdit: boolean;
}

const AuthorForm: React.FC<AuthorFormProps> = ({
  existingAuthor,
  onAddAuthor,
  onEditAuthor,
  loading,
  isEdit,
  onIsEdit
}) => {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<AuthorFormData>({
    resolver: zodResolver(authorSchema),
    defaultValues: {
      name: '',
      dateOfBirth: dateToString(new Date(new Date().getFullYear() - 18)),
      biography: '',
      citizenship: '',
      image: undefined,
    },
  });

  const navigate = useNavigate();
  const [localEdit, setLocalEdit] = useState<boolean>(isEdit);
  const [imagePreview, setImagePreview] = useState<string | null>(null);

  useEffect(() => {
    if (existingAuthor === undefined)
      setLocalEdit(true)
    else {
      setLocalEdit(isEdit)
    }
  }, [isEdit, existingAuthor])

  const loggedUser: User | null = getUserFromStorage();

  useEffect(() => {
    if (existingAuthor) {
      setValue('name', existingAuthor.name ?? undefined);
      setValue('biography', existingAuthor.biography);
      setValue('citizenship', existingAuthor.citizenship ?? undefined);
      setValue(
        'dateOfBirth',
        existingAuthor.dateOfBirth ? dateToString(existingAuthor.dateOfBirth) : '',
      );
    }
  }, [existingAuthor, setValue]);

  const onSubmit = (data: AuthorFormData) => {
    const formData = new FormData();
    formData.append("Name", data.name);
    formData.append("DateOfBirth", data?.dateOfBirth ? data.dateOfBirth : '',);
    formData.append("Biography", data.biography ?? '')
    formData.append("Citizenship", data.citizenship ?? '')
    if (data.image instanceof File) {
      formData.append("Image", data.image);
    }

    if(existingAuthor){
      formData.append("ImageUrl", existingAuthor.imageUrl ?? '');
    }

    if (existingAuthor) onEditAuthor(formData);
    else onAddAuthor(formData);
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
            <button className='form-back' onClick={() => navigate('/admin/booksRelated/authors')}>
              <img src={icons.oArrowLeft} />
            </button>
            <h1 className='text-2xl font-semibold'>Authors</h1>
          </div>
          {existingAuthor &&
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
                            <label
                        htmlFor='imageUpload'
                        className='flex items-center justify-center cursor-pointer
           overflow-hidden bg-contain bg-no-repeat bg-center w-[260px] h-[390px]'
                        style={{ backgroundImage: imagePreview ? `url(${imagePreview})` : existingAuthor?.imageUrl ? `url(${existingAuthor.imageUrl})` : "none", }}
                    >
                        {!existingAuthor?.imageUrl && (!imagePreview && <img className='w-[260px] h-[390px]' src={noImageUrl} />)}
                    </label>
                    <p>{errors.image?.message}</p>
        </div>
        <div className='flex flex-col gap-[33px] w-full'>
          <form onSubmit={handleSubmit(onSubmit)} className='flex flex-col gap-4 w-full'>
            <div className='input-row'>
              <label className='text-sm'>Name</label>
              <input {...register('name')}
                className='input-field'
                placeholder='Name'
                disabled={!localEdit} />
              <p>{errors.name?.message}</p>
            </div>
            <div className='flex gap-2'>
              <div className='input-row flex-1'>
                <label className='text-sm'>Date of birth</label>
                <input
                  className='input-field'
                  type='date'
                  {...register('dateOfBirth')}
                  placeholder='Date of Birth'
                  disabled={!localEdit}
                />
                <p>{errors.dateOfBirth?.message}</p>
              </div>
              <div className='input-row flex-1'>
                <label className='text-sm'>Citizenship</label>
                <input type='text' {...register('citizenship')}
                  className='input-field'
                  placeholder='Citizenship'
                  disabled={!localEdit} />
                <p>{errors.citizenship?.message}</p>
              </div>
            </div>
            <div className='input-row'>
              <label className='text-sm'>Biography</label>
              <textarea
                rows={20}
                className='input-field resize-none'
                {...register('biography')}
                placeholder='Biography'
                disabled={!localEdit}
              />
              <p>{errors.biography?.message}</p>
            </div>
            <button type='submit' disabled={loading} className='form-edit-btn fixed bottom-6 right-14'>
              {existingAuthor ? 'Save changes' : 'Add Author'}
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

export default AuthorForm;

