import React, { useState, useEffect } from "react";
import { BookView } from "../../../types";
import { Language } from "../../../types/subTypes/Book/Language"; // Adjust the path if necessary
import { CoverType } from "../../../types/subTypes/Book/CoverType";

interface BookAdminFormProps {
    book: BookView | null;
    onSubmit: (book: BookView) => void;
}

const BookAdminForm: React.FC<BookAdminFormProps> = ({ book, onSubmit }) => {
    const [title, setTitle] = useState(book ? book.title : "");
    const [year, setYear] = useState(book ? book.year : "");
    const [cover, setCover] = useState<CoverType>(book ? book.cover : CoverType.SOFT_COVER); 
    const [price, setPrice] = useState<number>(book ? book.price : 0);
    const [language, setLanguage] = useState<Language>(book ? book.language : Language.ENGLISH); 
    const [isAvailable, setIsAvailable] = useState(book ? book.isAvailable : false);
    const [startDate, setStartDate] = useState(book ? book.startDate : null);
    const [endDate, setEndDate] = useState(book ? book.endDate : null);
    const [discountRate, setDiscountRate] = useState(book ? book.discountRate : 0);

    useEffect(() => {
        if (book) {
            setTitle(book.title);
            setYear(book.year);
            setCover(book.cover);
            setPrice(book.price);
            setLanguage(book.language);
            setIsAvailable(book.isAvailable);
            setStartDate(book.startDate);
            setEndDate(book.endDate);
            setDiscountRate(book.discountRate);
        }
    }, [book]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const newBook: BookView = {
            bookId: book?.bookId || "defaultBookId",
            authorId: book?.authorId || "defaultAuthorId",
            publisherId: book?.publisherId || "defaultPublisherId",
            feedbackIds: book?.feedbackIds || [],
            title,
            year,
            cover,
            price,
            language,
            isAvailable,
            startDate,
            endDate,
            discountRate
        };
        onSubmit(newBook);
    };

    return (
        <form onSubmit={handleSubmit}>
            <div>
                <label>
                    Title:
                    <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} required />
                </label>
            </div>
            <div>
                <label>
                    Year:
                    <input type="number" value={year} onChange={(e) => setYear(e.target.value)} required />
                </label>
            </div>
            <div>
                <label>
                    Cover:
                    <select value={cover} onChange={(e) => setCover(e.target.value as CoverType)} required>
                        <option value="defaultCoverType">Default Cover</option>
                        <option value="hardcover">Hardcover</option>
                        <option value="paperback">Paperback</option>
                        <option value="ebook">Ebook</option>
                    </select>
                </label>
            </div>
            <div>
                <label>
                    Price:
                    <input type="number" value={price} onChange={(e) => setPrice(Number(e.target.value))} required />
                </label>
                <label>
                    Discount rate:
                    <input type="number" value={discountRate} onChange={(e) => setDiscountRate(Number(e.target.value))}/>
                </label>
                <label>
                    Start date:
                    <input type="date" value={startDate ? startDate.toISOString().split("T")[0] : ""} onChange={(e) => setStartDate(new Date(e.target.value))}/>
                </label>
                <label>
                    End date:
                    <input type="date" value={endDate ? endDate.toISOString().split("T")[0] : ""} onChange={(e) => setEndDate(new Date(e.target.value))}/>
                </label>
            </div>
            <div>
                <label>
                    Language:
                    <select value={language} onChange={(e) => setLanguage(e.target.value as Language)} required>
                        <option value="defaultLanguage">Default Language</option>
                        <option value="english">English</option>
                        <option value="spanish">Spanish</option>
                        <option value="french">French</option>
                        <option value="german">German</option>
                    </select>
                </label>
            </div>
            <div>
                <label>
                    Available:
                    <input type="checkbox" checked={isAvailable} onChange={(e) => setIsAvailable(e.target.checked)} />
                </label>
            </div>
            <button type="submit">Submit</button>
        </form>
    );
};

export default BookAdminForm;