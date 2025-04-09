import { useEffect, useState } from "react";

interface PaginationProps {
	pagination: { pageNumber: number; pageSize: number; totalCount: number }
	onPageChange: (pageNumber: number) => void
}

const Pagination: React.FC<PaginationProps> = ({ pagination, onPageChange }) => {
	const totalPages = Math.ceil(pagination.totalCount / pagination.pageSize)
	const [pages,setPages] = useState<number[]>([])

	useEffect(() => {
		setPages([]);
		for(let i = Math.max(pagination.pageNumber - 1, 1); i < Math.min(pagination.pageNumber + 2, totalPages); i++ ){
			setPages((prev) => [...prev, i]);
		}
	},[pagination,onPageChange, totalPages])

	return (
		<div style={{display:"flex"}}>
			{pagination.pageNumber > 2 && (
				<button onClick={() => onPageChange(pagination.pageNumber - 1)}>&lt;</button>
			)}
			{pages.map((page) => (
				<button onClick={() => onPageChange(page)}>{page == pagination.pageNumber ? (<strong>{page}</strong>) : (page)}</button>
			))}
			<p>{pages[pages.length - 1] + 1 < totalPages ? "..." : ""}</p>
			{pages[pages.length - 1] < totalPages ? (
				<button onClick={() => onPageChange(totalPages)}>
					{pagination.pageNumber == totalPages ? (<strong>{totalPages}</strong>) : (totalPages)}
				</button>
			) : <></>}
			{pagination.pageNumber < totalPages && (
				<button onClick={() => onPageChange(pagination.pageNumber + 1)}>&gt;</button>
			)}
		</div>
	)
}

export default Pagination
