import React, { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { SubCategoryFormData } from '@/utils';
import { SubCategoryFormDataToSubCategory } from '@/api/adapters/bookAdapter';
import { addSubCategoryService } from '@/services';
import SubCategoryForm from '@/components/book/admin/SubCategoryForm';

interface SubCategoryContainerProps{
    categoryId: string;
}

const SubCategoryFormContainer: React.FC<SubCategoryContainerProps> = ({categoryId}) => {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleNavigate = useCallback(
    (route: string) => navigate(route),
    [navigate],
  );

  const handleMessage = useCallback(
    (message: string, type: 'success' | 'error') => {
      dispatch(addNotification({ message, type }));
    },
    [dispatch],
  );

  const handleAddSubCategory = useCallback(
    async (categoryForm: SubCategoryFormData) => {
      const category = SubCategoryFormDataToSubCategory(categoryForm);
      const response = await addSubCategoryService(category);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('SubCategory created successfully!', 'success');
        handleNavigate('/admin/booksRelated/categories');
      }
    },
    [handleMessage, handleNavigate],
  );

  return (
    <SubCategoryForm
      onAddSubCategory={handleAddSubCategory}
      categoryId={categoryId}
    />
  );
};

export default SubCategoryFormContainer;
