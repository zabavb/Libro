import BookDetailsContainer from "@/containers/books/BookDetailsContainer"
import { useNavigate, useParams } from "react-router-dom"


const BookDetailsPage = () => {
    const { bookId } = useParams<{bookId: string}>()
    const navigate = useNavigate()
    

    const handleGoBack = () => {
        navigate("/catalog")
    }

    return (
        <div>
            <header>
                <button onClick={() => handleGoBack()}>Back</button>
                <h2>Details</h2>
            </header>
            <main>
                <BookDetailsContainer bookId={bookId ?? ""} />
            </main>
        </div>
    )
}

export default BookDetailsPage