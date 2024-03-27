import { createContext } from "react";
import { RegisterService } from "../../api/RegisterApi/RegisterApi";

export const RegisterApiContext = createContext<RegisterService>(null!)