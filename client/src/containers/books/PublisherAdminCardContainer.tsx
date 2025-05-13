import { useDispatch } from "react-redux";
import { AppDispatch } from "../../state/redux";
import { useNavigate } from "react-router-dom";
import { addNotification } from "../../state/redux/slices/notificationSlice";
import { Publisher } from "@/types/types/book/Publisher";
import { removePublisherService } from "@/services";
import PublisherAdminCard from "@/components/book/admin/PublisherAdminCard";

interface PublisherAdminCardContainerProps{
    publisher: Publisher
}

const PublisherAdminCardContainer: React.FC<PublisherAdminCardContainerProps> = ({publisher}) => {
    const dispatch = useDispatch<AppDispatch>()
    const navigate = useNavigate()

    const handleDelete = async (e: React.MouseEvent) => {
        e.stopPropagation()
        const response = await removePublisherService(publisher.publisherId)
        dispatch(
              response.error
                ? addNotification({
                    message: response.error,
                    type: 'error',
                  })
                : addNotification({
                    message: 'Publisher successfully deleted.',
                    type: 'success',
                  }),
            );
    }

    const handleNavigate = () => {
        navigate(`/admin/booksRelated/publisher/${publisher.publisherId}`)
    }

    return (
        <PublisherAdminCard 
        onDelete={handleDelete} 
        onNavigate={handleNavigate}
        publisher={publisher}/>
    )
}

export default PublisherAdminCardContainer