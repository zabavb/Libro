import SubCategoryFormContainer from '@/containers/books/SubCategoryFormContainer';
import { useNavigate, useParams } from 'react-router-dom';

const SubCategoryFormPage = () => {
    const { categoryId } = useParams<{ categoryId: string }>();
    const navigate = useNavigate();
    if(!categoryId) navigate('/admin/bookRelated/categories') 
    return (
    <div>
      <main>
        <SubCategoryFormContainer categoryId={categoryId ?? ''} />
      </main>
    </div>
  );
};

export default SubCategoryFormPage;
