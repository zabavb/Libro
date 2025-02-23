import { CoverType } from "../subTypes/Book/CoverType"
import { Language } from "../subTypes/Book/Language"

export interface BookView {
	bookId: string
	title: string
	authorId: string
	publisherId: string
	price: number
	language: Language
	year: string
	description?: string
	cover: CoverType
	isAvailable: boolean
	feedbackIds: string[]
	//subcategoryIds: string[]
}
