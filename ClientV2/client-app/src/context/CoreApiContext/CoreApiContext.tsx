import { createContext } from "react"
import { ApiService } from "../../api/CoreApi/CoreApi"

const apiService = new ApiService()

export const ApiContext = createContext(apiService)

export const ApiProvider = ({children}: any) => {
  return (
    <ApiContext.Provider value={apiService}>
      {children}
    </ApiContext.Provider>
  )
}