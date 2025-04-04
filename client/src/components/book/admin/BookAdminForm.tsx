import React, { useState, useEffect } from "react";
import { BookView } from "../../../types";
import { Language } from "../../../types/subTypes/book/Language"; 
import { CoverType } from "../../../types/subTypes/book/CoverType";

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

    useEffect(() => {
        if (book) {
            setTitle(book.title);
            setYear(book.year);
            setCover(book.cover);
            setPrice(book.price);
            setLanguage(book.language);
            setIsAvailable(book.isAvailable);
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
        };
        onSubmit(newBook);
    };

    const renderEnumOptions = (enumObj: any) => {
        return Object.entries(enumObj).map(([key, value]) => (
            <option key={key} value={value as string | number}>
                {value as string | number}
            </option>
        ));
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
                        {renderEnumOptions(CoverType)}
                    </select>
                </label>
            </div>
            <div>
                <label>
                    Price:
                    <input type="number" value={price} onChange={(e) => setPrice(Number(e.target.value))} required />
                </label>
            </div>
            <div>
                <label>
                    Language:
                    <select value={language} onChange={(e) => setLanguage(e.target.value as Language)} required>
                        {renderEnumOptions(Language)}
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
