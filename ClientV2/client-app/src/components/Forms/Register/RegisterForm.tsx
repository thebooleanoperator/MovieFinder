import {
  Button, 
  Card,
  CardActions,
  CardContent,
  Typography } from "@mui/material"
import { Form } from "react-final-form"
import { MuiTextField } from "../../UI/MuiTextField"
import { isValidEmail, minLength, required } from "../../../validators/fieldValidators"
import { composeValidators } from "../../../validators/coreValidators"
import { RegisterApiContext } from "../../../context/RegisterApiContext/RegisterContext"
import { useContext } from "react"

type RegisterFormState = {
  email: string,
  password: string
}

type RegisterFormProps = {
  handleError: (errorMessage: string) => void
}

export const RegisterForm: React.FC<RegisterFormProps> = ({ handleError }) => {
  const registerService = useContext(RegisterApiContext)

  const handleRegister = async (values: RegisterFormState) => {
    var response = await registerService.Register(values.email, values.password)
    console.log(response)
    if (!response.isSuccess) {
      handleError('Register Failed')
    }
  }

  return (
    <Card variant="outlined" sx={{maxWidth: "50%"}}>
      <CardContent>
        <Typography gutterBottom variant="h4" component="div">
          Register
        </Typography>
        <Form
          onSubmit={handleRegister}
          render={({handleSubmit}) => (
          <form onSubmit={handleSubmit}>
            <div style={{display: "flex", flexDirection:"column", maxWidth: "75%"}}>
              <MuiTextField name="email" label="email" validate={composeValidators(required, isValidEmail)} />
              <MuiTextField 
                name="password" 
                label="password" 
                validate={
                  composeValidators(
                    required, 
                    minLength(8, 'Password must contain at least 8 chars'))
                } />
            </div>
            <CardActions>
              <Button type="submit" variant="contained">Register</Button>
            </CardActions>
          </form>
          )}>
        </Form>
      </CardContent>
    </Card>
  )
}