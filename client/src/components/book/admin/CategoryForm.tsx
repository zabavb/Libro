import React from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { icons } from '@/lib/icons'
import "@/assets/styles/components/user/user-form.css"
import { getUserFromStorage } from '@/utils/storage';
import { useNavigate } from 'react-router-dom';
import { User } from '@/types';
import { CategoryFormData, categorySchema } from '@/utils';
interface CategoryFormProps {
  onAddCategory: (category: CategoryFormData) => Promise<void>;
}

const CategoryForm: React.FC<CategoryFormProps> = ({
  onAddCategory,
}) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CategoryFormData>({
    resolver: zodResolver(categorySchema),
    defaultValues: {
      name: '',
    },
  });

  const navigate = useNavigate();

  const loggedUser: User | null = getUserFromStorage();

  const onSubmit = (data: CategoryFormData) => {
    onAddCategory(data);
  };

  return (
    <div>
      <header className='header-container'>
        <div className='flex gap-[60px] items-center'>
          <div className='flex gap-5 items-center'>
            <button className='form-back' onClick={() => navigate('/admin/booksRelated/categories')}>
              <img src={icons.oArrowLeft} />
            </button>
            <h1 className='text-2xl font-semibold'>Categories</h1>
          </div>


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
                placeholder='Name' />
              <p>{errors.name?.message}</p>
            </div>
            <button type='submit' className='form-edit-btn fixed bottom-6 right-14'>
              Add Category
            </button>
          </form>
        </div>
      </main>
    </div>
  );
};

export default CategoryForm;

