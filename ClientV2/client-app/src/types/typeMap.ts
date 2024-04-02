export type TypeMap<T extends { [index: string]: unknown }> = {
  [Key in keyof T]: T[Key] extends undefined 
    ? {type: Key}
    : {type:Key, payload: T[Key]}
}