import { User } from "@/types"

export const getUserFromStorage = (): User | null => {
    try {
      const userData = localStorage.getItem('user')
      if (!userData) return null
  
      return JSON.parse(userData) as User
    } catch (error) {
      console.error('Error parsing user data from localStorage:', error)
      return null
    }
  }