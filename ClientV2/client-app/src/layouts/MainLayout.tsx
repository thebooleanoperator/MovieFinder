import { BottomNavigation, BottomNavigationAction } from "@mui/material"
import { LOGIN, REGISTER } from "../constants/routeConstants"
import { AppRegistration, Login as MuiLogin } from "@mui/icons-material"
import { Outlet } from "react-router-dom"

export const MainLayout = () =>{
  // get isLoggedIn from context
  
  return(
      <>
        <Outlet />
        <BottomNavigation showLabels>
          <BottomNavigationAction label="Register" href={REGISTER} icon={<AppRegistration />} />
          <BottomNavigationAction label="Login" href={LOGIN} icon={<MuiLogin />} />
        </BottomNavigation>
      </>
  )
}