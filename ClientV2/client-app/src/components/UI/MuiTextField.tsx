import { Field } from 'react-final-form';
import { TextField } from '@mui/material';

export const MuiTextField = ({ name, label }: any) => (
  <Field name={name}>
    {({ input, meta }) => (
      <TextField
        {...input}
        label={label}
        error={meta.touched && meta.error}
        helperText={meta.touched && meta.error ? meta.error : ' '}
      />
    )}
  </Field>
);
