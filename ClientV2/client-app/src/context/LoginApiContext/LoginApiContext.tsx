import { createContext } from "react"
import { LoginService } from "../../api/LoginApi/LoginApi"

export const LoginApiContext = createContext<LoginService>(null!);