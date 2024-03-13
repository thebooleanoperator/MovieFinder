import { Outlet } from "react-router-dom"
import { MuiAuthAppBar } from "../components/UI/MuiAuthAppBar"

export const MainLayout = () =>{
  // get isLoggedIn from context
  // create and toggle "signed in app bar"
  // also toggle mui auth app bar

  return(
      <>
        <MuiAuthAppBar />
        <Outlet />
      </>
  )
}