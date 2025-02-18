import { createContext, useContext, useEffect, useState } from "react"
import { loginService, getMeService } from "../../services"
import { Login, User } from "../../types"

interface AuthContextProps {
	user: User | null
	token: string | null
	login: (data: Login) => Promise<void>
	logout: () => void
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined)

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
	const [user, setUser] = useState<User | null>(null)
	const [token, setToken] = useState<string | null>(localStorage.getItem("token"))

	useEffect(() => {
		if (!token) return
		;(async () => {
			try {
				if (token) {
					const currentUser = await getMeService(token)
					setUser(currentUser)
				}
			} catch {
				logout()
			}
		})()
	}, [])

	const login = async (userData: Login) => {
		try {
			const data = await loginService(userData)

			localStorage.setItem("token", data.Token)
			localStorage.setItem("token_expiry", String(Date.now() + data.ExpiresInMinutes * 60 * 1000))

			setToken(data.Token)

			const currentUser = await getMeService(data.Token)
			setUser(currentUser)
		} catch (error) {
			console.error("Login failed:", error)
			throw new Error("Invalid credentials. Please try again.")
		}
	}

	const logout = () => {
		localStorage.removeItem("token")
		localStorage.removeItem("token_expiry")
		setUser(null)
		setToken(null)
	}

	return <AuthContext.Provider value={{ user, token, login, logout }}>{children}</AuthContext.Provider>
}

export const useAuth = () => {
	const context = useContext(AuthContext)
	if (!context) throw new Error("useAuth must be used within an AuthProvider")
	return context
}
