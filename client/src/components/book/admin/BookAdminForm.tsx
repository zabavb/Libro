import React, { useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { BookView } from "../../../types";
import { bookValidationSchema, BookFormData } from "../../../utils/bookValidationSchema";
import { CoverType } from "../../../types/subTypes/book/CoverType";
import { Language } from "../../../types/subTypes/book/Language";

interface BookAdminFormProps {
    book: BookView | null;
    onSubmit: (book: BookView) => void;
}

const BookAdminForm: React.FC<BookAdminFormProps> = ({ book, onSubmit }) => {
    const {
        register,
        handleSubmit,
        setValue,
        formState: { errors },
    } = useForm<BookFormData>({
        resolver: zodResolver(bookValidationSchema),
        defaultValues: {
            title: "",
            year: "2024",
            cover: CoverType.SOFT_COVER,
            price: 0,
            language: Language.ENGLISH,
            isAvailable: false,
        },
    });

    useEffect(() => {
        if (book) {
            setValue("title", book.title);
            setValue("year", book.year);
            setValue("cover", book.cover);
            setValue("price", book.price);
            setValue("language", book.language);
            setValue("isAvailable", book.isAvailable);
        }
    }, [book, setValue]);

    const submitForm = (data: BookFormData) => {
        const newBook: BookView = {
            bookId: book?.bookId || "defaultBookId",
            authorId: book?.authorId || "defaultAuthorId",
            publisherId: book?.publisherId || "defaultPublisherId",
            feedbackIds: book?.feedbackIds || [],
            ...data,
        };

        onSubmit(newBook);
    };

    return (
        <form onSubmit={handleSubmit(submitForm)}>
            <div>
                <label>Title:</label>
                <input {...register("title")} />
                <p>{errors.title?.message}</p>
            </div>

            <div>
                <label>Year:</label>
                <input type="number" {...register("year")} />
                <p>{errors.year?.message}</p>
            </div>

            <div>
                <label>Cover:</label>
                <select {...register("cover")}>
                    {Object.entries(CoverType).map(([key, value]) => (
                        <option key={key} value={value}>
                            {value}
                        </option>
                    ))}
                </select>
                <p>{errors.cover?.message}</p>
            </div>

            <div>
                <label>Price:</label>
                <input type="number" {...register("price")} />
                <p>{errors.price?.message}</p>
            </div>

            <div>
                <label>Language:</label>
                <select {...register("language")}>
                    {Object.entries(Language).map(([key, value]) => (
                        <option key={key} value={value}>
                            {value}
                        </option>
                    ))}
                </select>
                <p>{errors.language?.message}</p>
            </div>

            <div>
                <label>Available:</label>
                <input type="checkbox" {...register("isAvailable")} />
            </div>

            <button type="submit">Submit</button>
        </form>
    );
};

export default BookAdminForm;
