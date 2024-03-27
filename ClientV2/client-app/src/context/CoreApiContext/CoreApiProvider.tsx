import { ApiService } from "../../api/CoreApi/CoreApi"
import { ApiContext } from "./CoreApiContext"

export const ApiProvider = ({children}: any) => {
  const defaultApiService = new ApiService()
  return (
    <ApiContext.Provider value={defaultApiService}>
      {children}
    </ApiContext.Provider>
  )
}