import { Alert } from '@mui/material'
import { RegisterForm } from '../../components/Forms/Register/RegisterForm'
import { useState } from 'react'

export const RegisterPage = () => {
  const [errorMessage, setErrorMessage] = useState('')

  const handleError = (errorMessage: string) => {
    setErrorMessage(errorMessage)
  }
  
  return (
    <>
      { errorMessage && <Alert severity="error">{errorMessage}</Alert> }
      <RegisterForm handleError={handleError} />
    </>
  )
}
