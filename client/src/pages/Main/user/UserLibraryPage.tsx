import React, { useState } from 'react';
import './LibraryPage.css';

interface Book {
    id: string;
    title: string;
    authorFirstName: string;
    authorLastName: string;
    coverImageUrl: string;
    bookType: 'audio' | 'electronic';
    isPlaceholder?: boolean;
}

const mockBooks: Book[] = [
    {
        id: '1',
        title: 'Дисципліна – це свобода',
        authorFirstName: 'Джоко',
        authorLastName: 'Віллінк',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover1&font=roboto',
        bookType: 'audio',
    },
    {
        id: '2',
        title: 'Танці з кістками',
        authorFirstName: 'Андрій',
        authorLastName: 'Сем\'янків',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover2&font=roboto',
        bookType: 'audio',
    },
    {
        id: '3',
        title: 'Служниця',
        authorFirstName: 'Фріда',
        authorLastName: 'Макфадден',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover3&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '4',
        title: 'Стіни в моїй голові',
        authorFirstName: 'Володимир',
        authorLastName: 'Станчишин',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover4&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '5',
        title: 'Аудіокнига Приклад',
        authorFirstName: 'Автор',
        authorLastName: 'П\'ять',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover5&font=roboto',
        bookType: 'audio',
    },
    {
        id: '6',
        title: 'Електронна Книгаadsasdadasdsadsdsadsadsasadsadadssd',
        authorFirstName: 'Авторasd sad a ssa sa asdsad',
        authorLastName: 'Шістьdsasadsdasasddsds',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover6&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '7',
        title: 'Електронна Книга2',
        authorFirstName: 'Автор',
        authorLastName: '7',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover6&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '8',
        title: 'Електронна Книга3',
        authorFirstName: 'Автор',
        authorLastName: '8',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover6&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '9',
        title: 'Електронна Книга4',
        authorFirstName: 'Автор',
        authorLastName: '9',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover6&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '10',
        title: 'Електронна Книга4',
        authorFirstName: 'Автор',
        authorLastName: '10',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover6&font=roboto',
        bookType: 'electronic',
    },
    {
        id: '11',
        title: 'Електронна Книга5',
        authorFirstName: 'Автор',
        authorLastName: '11',
        coverImageUrl: 'https://placehold.co/150x220/D3D3D3/000000?text=Cover6&font=roboto',
        bookType: 'electronic',
    },
];


const LibraryPage: React.FC = () => {
    const [currentFilter, setCurrentFilter] = useState<'audio' | 'electronic'>('audio');
    const filteredBooks = mockBooks.filter(book => book.bookType === currentFilter);

    return (
        <div className="library-page-wrapper">
            <div className="library-header">
                <h1>Бібліотека</h1>
            </div>

            <div className="library-content-shell">
                <nav className="sidebar-navigation">
                    <button
                        className={`nav-button ${currentFilter === 'audio' ? 'active' : ''}`}
                        onClick={() => setCurrentFilter('audio')}
                    >
                        Аудіокнига
                    </button>
                    <button
                        className={`nav-button ${currentFilter === 'electronic' ? 'active' : ''}`}
                        onClick={() => setCurrentFilter('electronic')}
                    >
                        Електронна
                    </button>
                </nav>

               
                <main className="book-display-area">
                    {filteredBooks.length > 0 ? (
                        <div className="books-grid">
                            {filteredBooks.map((book) => (
                                <div key={book.id} className="book-item">
                                    <div className="book-cover-container">
                                        <img src={book.coverImageUrl} alt={book.title} />
                                    </div>
                                    <p className="book-item-title">{book.title}</p>
                                    <p className="book-item-author">{`${book.authorFirstName} ${book.authorLastName}`}</p>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="no-books-available-message">Немає книг у цій категорії.</p>
                    )}
                </main>
            </div>
        </div>
    );
};

export default LibraryPage;