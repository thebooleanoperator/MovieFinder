import { validEmail } from "../constants/regex";

export const required = (value: any) => (value ? undefined : 'Required');

export const minLength = (minLength: number, message: string) => (value: string) => (
  value.length >= minLength ? undefined : message
)

export const isValidEmail = (value: string) => (
  value.match(validEmail) ? undefined : 'Invalid Email'
)