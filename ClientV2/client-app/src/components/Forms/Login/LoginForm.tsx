import { useContext } from "react"
import { LoginApiContext } from "../../../context/LoginApiContext/LoginApiContext"
import { Form} from 'react-final-form'
import {
  Button, 
  Card,
  CardActions,
  CardContent,
  Typography } from "@mui/material"
import { MuiTextField } from "../../UI/MuiTextField"
import { required } from "../../../validators/fieldValidators"

type LoginFormState = {
  email: string,
  password: string
}

type LoginFormProps = {
  handleError: (errorMessage: string) => void
}

export const LoginForm: React.FC<LoginFormProps> = ({ handleError }) => {
  const loginService = useContext(LoginApiContext)

  const handleLogin = async (values: LoginFormState) => {
    const response = await loginService.Login(values.email, values.password)
    console.log(response)
    if (!response.isSuccess) {
      handleError('Sign In Failed')
    }
  }

  const handleGuestLogin = async () => {
    const response = await loginService.LoginAsGuest()
    console.log(response)
    if (!response.isSuccess) {
      handleError('Guest Login Failed')
    }
  }

  return (
    <Card variant="outlined" sx={{maxWidth: "50%"}}>
      <CardContent>
        <Typography gutterBottom variant="h4" component="div">
          Sign In
        </Typography>
        <Form
          onSubmit={handleLogin}
          render={({handleSubmit}) => (
          <form onSubmit={handleSubmit}>
            <div style={{display: "flex", flexDirection:"column", maxWidth: "75%"}}>
              <MuiTextField name="email" label="email" validate={required} />
              <MuiTextField name="password" label="password" validate={required} />
            </div>
            <CardActions>
              <Button type="submit" variant="contained">Sign In</Button>
              <Button type="button" variant="contained" onClick={async () => {await handleGuestLogin()}}>Guest Login</Button>
            </CardActions>
          </form>
          )}>
        </Form>
      </CardContent>
    </Card>
  )
}