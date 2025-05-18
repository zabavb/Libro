import React, { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { icons } from '@/lib/icons'
import "@/assets/styles/components/user/user-form.css"
import { getUserFromStorage } from '@/utils/storage';
import { useNavigate } from 'react-router-dom';
import { User } from '@/types';
import { PublisherFormData, publisherSchema } from '@/utils';
import { Publisher } from '@/types/types/book/Publisher';
interface PublisherFormProps {
  existingPublisher?: Publisher;
  onAddPublisher: (publisher: PublisherFormData) => Promise<void>;
  onEditPublisher: (
    updatedPublisher: PublisherFormData,
  ) => Promise<void>;
  loading: boolean;
  onIsEdit: (isEdit: boolean) => void;
  isEdit: boolean;
}

const PublisherForm: React.FC<PublisherFormProps> = ({
  existingPublisher,
  onAddPublisher,
  onEditPublisher,
  loading,
  isEdit,
  onIsEdit
}) => {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm<PublisherFormData>({
    resolver: zodResolver(publisherSchema),
    defaultValues: {
      name: '',
      description: '',
    },
  });

  const navigate = useNavigate();
  const [localEdit, setLocalEdit] = useState<boolean>(isEdit);

  useEffect(() => {
    if (existingPublisher === undefined)
      setLocalEdit(true)
    else {
      setLocalEdit(isEdit)
    }
  }, [isEdit, existingPublisher])

  const loggedUser: User | null = getUserFromStorage();

  useEffect(() => {
    if (existingPublisher) {
      setValue('name', existingPublisher.name ?? undefined);
      setValue('description', existingPublisher.description);
    }
  }, [existingPublisher, setValue]);

  const onSubmit = (data: PublisherFormData) => {
    if (existingPublisher) onEditPublisher(data);
    else onAddPublisher(data);
  };

  return (
    <div>
      <header className='header-container'>
        <div className='flex gap-[60px] items-center'>
          <div className='flex gap-5 items-center'>
            <button className='form-back' onClick={() => navigate('/admin/booksRelated/publishers')}>
              <img src={icons.oArrowLeft} />
            </button>
            <h1 className='text-2xl font-semibold'>Publishers</h1>
          </div>
          {existingPublisher &&
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
        <div className='flex flex-col gap-[33px] w-full'>
          <form onSubmit={handleSubmit(onSubmit)} className='flex flex-col gap-4 w-full'>
            <div className='input-row'>
              <label className='text-sm'>Name</label>
              <input {...register('name')}
                className='input-field'
                placeholder='Name' 
                disabled={!localEdit}/>
              <p>{errors.name?.message}</p>
            </div>
            <div className='input-row'>
              <label className='text-sm'>Publisher description</label>
              <textarea
                rows={20}
                className='input-field resize-none'
                {...register('description')}
                placeholder='Description'
                disabled={!localEdit}
              />
              <p>{errors.description?.message}</p>
            </div>
            <button type='submit' disabled={loading} className='form-edit-btn fixed bottom-6 right-14'>
              {existingPublisher ? 'Save changes' : 'Add Publisher'}
            </button>
          </form>
        </div>
      </main>
    </div>
  );
};

export default PublisherForm;

