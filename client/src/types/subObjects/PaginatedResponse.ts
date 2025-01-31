export interface PaginatedResponse<T> {
	items: T[]
	pageNumber: 1
	pageSize: 10
	totalCount: 1
}
