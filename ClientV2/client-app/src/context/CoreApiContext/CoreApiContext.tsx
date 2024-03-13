import { createContext } from "react"
import { ApiService } from "../../api/CoreApi/CoreApi"

export const ApiContext = createContext<ApiService>(null!)