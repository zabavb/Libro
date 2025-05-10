import { useDispatch } from "react-redux";
import { Author } from "../../types";
import { AppDispatch } from "../../state/redux";
import { useNavigate } from "react-router-dom";
import { addNotification } from "../../state/redux/slices/notificationSlice";
import { deleteAuthorService } from "@/services/authorService";
import AuthorAdminCard from "@/components/book/admin/AuthorAdminCard";

interface AuthorAdminCardContainerProps{
    author: Author
}

const AuthorAdminCardContainer: React.FC<AuthorAdminCardContainerProps> = ({author}) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()

    const handleDelete = async (e: React.MouseEvent) => {
        e.stopPropagation()
        const response = await deleteAuthorService(author.id)
        dispatch(
              response.error
                ? addNotification({
                    message: response.error,
                    type: 'error',
                  })
                : addNotification({
                    message: 'Author successfully deleted.',
                    type: 'success',
                  }),
            );
    }

    const handleNavigate = () => {
        navigate(`/admin/bookRelated/author/${author.id}`)
    }

    return (
        <AuthorAdminCard
            author={author} 
            onNavigate={handleNavigate}
            onDelete={handleDelete}
            />
    )
}

export default AuthorAdminCardContainer