export const composeValidators = (...validators: any) => (value: any) => (
  validators.reduce((error: string, validator: (value: any) => undefined | string) => error || validator(value), undefined)
)