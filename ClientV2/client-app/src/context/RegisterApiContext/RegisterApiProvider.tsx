import { useContext } from "react"
import { ApiContext } from "../CoreApiContext/CoreApiContext"
import { RegisterApiContext } from "./RegisterContext";
import { RegisterService } from "../../api/RegisterApi/RegisterApi";

type RegisterApiProviderProps = {
  children: React.ReactNode;
}

export const RegisterApiProvider: React.FC<RegisterApiProviderProps> = ({children}) => {
  const apiService = useContext(ApiContext)
  const registerService = new RegisterService(apiService)

  return (
    <RegisterApiContext.Provider value={registerService}>
      {children}
    </RegisterApiContext.Provider>
  )
}