import { ReactNode } from "react";
import { LoginApiProvider } from "./LoginApiContext/LoginApiProvider"
import { RegisterApiProvider } from "./RegisterApiContext/RegisterApiProvider"
import { UserProvider } from "./UserContext/UserProvider";

type RootProviderProps = {
  children: ReactNode;
};

export const RootProvider: React.FC<RootProviderProps> = ({children}) => (
  <LoginApiProvider>
    <RegisterApiProvider>
      <UserProvider>
        {children}
      </UserProvider>
    </RegisterApiProvider>
  </LoginApiProvider>
)