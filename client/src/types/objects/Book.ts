
export interface Book {
	bookId: string
	title: string
	authorId: string
	publisherId: string
	price: number
	language: number
	year: Date
	description?: string
	cover: number
	isAvailable: boolean
	feedbackIds: string[]
	discountRate: number;
	startDate: Date;
	endDate: Date;
	//subCategoryIds: string[]
}
