import { useEffect, useState } from "react";
import "@/assets/styles/components/pagination.css"
interface PaginationProps {
	pagination: { pageNumber: number; pageSize: number; totalCount: number }
	onPageChange: (pageNumber: number) => void
}

const Pagination: React.FC<PaginationProps> = ({ pagination, onPageChange }) => {
	const totalPages = Math.ceil(pagination.totalCount / pagination.pageSize)
	const [pages, setPages] = useState<number[]>([])

	useEffect(() => {
		setPages([]);
		for (let i = Math.max(pagination.pageNumber - 1, 1); i < Math.min(pagination.pageNumber + 2, totalPages); i++) {
			setPages((prev) => [...prev, i]);
		}
	}, [pagination, onPageChange, totalPages])

	return (
		<div className="pagination-container">

				<button
					onClick={() => onPageChange(Math.max(1, pagination.pageNumber - 1))}
					className="page-nav">
				{totalPages > 1 ? (<p>&lt;</p>) : (<p></p>)}
				</button>

			<div className="number-container">
				{pages.map((page) => (
					<button onClick={() => onPageChange(page)}>
						{page == pagination.pageNumber ?
							(<p className="page-number active">{page}</p>)
							:
							(<p className="page-number">{page}</p>)}
					</button>
				))}

				<p>{pages[pages.length - 1] + 1 < totalPages ? "..." : ""}</p>
				{pages[pages.length - 1] < totalPages ? (
					<button onClick={() => onPageChange(totalPages)}>
						{pagination.pageNumber == totalPages ? (
							<p className="page-number active">
								{totalPages}</p>)
							:
							(<p className="page-number">{totalPages}</p>)
						}
					</button>
				) : <></>}
			</div>
			<button
				onClick={() => onPageChange(Math.min(totalPages, pagination.pageNumber + 1))}
				className="page-nav"
			>
				{totalPages > 1 ? (<p>&gt;</p>) : (<p></p>)}
			</button>


		</div>
	)
}

export default Pagination
