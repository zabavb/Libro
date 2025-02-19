interface PaginationProps {
	pagination: { pageNumber: number; pageSize: number; totalCount: number }
	onPageChange: (pageNumber: number) => void
}

const Pagination: React.FC<PaginationProps> = ({ pagination, onPageChange }) => {
	const totalPages = Math.ceil(pagination.totalCount / pagination.pageSize)

	return (
		<div>
			<span>Pages: </span>
			{pagination.pageNumber > 1 && (
				<button onClick={() => onPageChange(pagination.pageNumber - 1)}>Previous</button>
			)}
			<span>{pagination.pageNumber} </span>
			{pagination.pageNumber < totalPages && (
				<button onClick={() => onPageChange(pagination.pageNumber + 1)}>Next</button>
			)}
		</div>
	)
}

export default Pagination
