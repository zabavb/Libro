import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { PaginatedResponse, Book } from "../../../types";
import { fetchBooksService } from "../../../services/bookService";

export const fetchBooks = createAsyncThunk(
  "books/fetchBooks",
  async ({
    pageNumber = 1,
    pageSize = 10,
  }: {
    pageNumber?: number;
    pageSize?: number;
  }): Promise<PaginatedResponse<Book>> => {
    try {
      return await fetchBooksService(pageNumber, pageSize);
    } catch (error) {
      throw new Error("Failed to fetch books.");
    }
  }
);

const initialState = {
  data: [] as Book[],
  loading: false,
  error: null as string | null,
  operationStatus: null as "success" | "error" | "pending" | null,
  pagination: {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  },
};

const booksSlice = createSlice({
  name: "books",
  initialState,
  reducers: {
    resetOperationStatus: (state) => {
      state.operationStatus = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchBooks.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.operationStatus = "pending"; 
      })
      .addCase(fetchBooks.fulfilled, (state, action) => {
        state.loading = false;
        state.operationStatus = "success"; 
        state.data = action.payload.items;
        state.pagination = {
          pageNumber: action.payload.pageNumber,
          pageSize: action.payload.pageSize,
          totalCount: action.payload.totalCount,
        };
      })
      .addCase(fetchBooks.rejected, (state, action) => {
        state.loading = false;
        state.operationStatus = "error"; 
        state.error = action.error.message ?? "Failed to fetch books.";
        console.error("Error fetching books:", action.error.message);
      });
  },
});

export const { resetOperationStatus } = booksSlice.actions;

export default booksSlice.reducer;
