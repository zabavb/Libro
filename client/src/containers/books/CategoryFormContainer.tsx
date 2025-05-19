import { useCallback } from 'react';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { addNotification } from '../../state/redux/slices/notificationSlice';
import { CategoryFormData } from '@/utils';
import { CategoryFormDataToCategory } from '@/api/adapters/bookAdapter';
import { addCategoryService } from '@/services';
import CategoryForm from '@/components/book/admin/CategoryForm';


const CategoryFormContainer = () => {
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

  const handleAddCategory = useCallback(
    async (categoryForm: CategoryFormData) => {
      const category = CategoryFormDataToCategory(categoryForm);
      const response = await addCategoryService(category);

      if (response.error) handleMessage(response.error, 'error');
      else {
        handleMessage('Category created successfully!', 'success');
        handleNavigate('/admin/booksRelated/categories');
      }
    },
    [handleMessage, handleNavigate],
  );

  return (
    <CategoryForm
      onAddCategory={handleAddCategory}
    />
  );
};

export default CategoryFormContainer;
