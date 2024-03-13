import { useState } from "react"
import { LoginForm } from "../../components/Forms/Login/LoginForm"
import { Alert } from "@mui/material"

export const LoginPage: React.FC = () => {
  const [errorMessage, setErrorMessage] = useState('')

  const handleError = (errorMessage: string) => {
    setErrorMessage(errorMessage)
  }
  
  return (
    <>
      { errorMessage && <Alert severity="error">{errorMessage}</Alert> }
      <LoginForm handleError={handleError} />
    </>
  )
}