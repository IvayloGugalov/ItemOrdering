import { useState, useEffect } from "react";

const getLocalValue = (key, initValue) => {
  // Server side react Next.js
  if (typeof window === 'undefined') return initValue;

  // if value stored
  const localValue = JSON.parse(localStorage.getItem(key));
  if (localValue) return localValue;

  // return result from a function
  if (initValue instanceof Function) return initValue();

  return initValue;
}

const useLocalStorage = (key, initValue) => {
  const [value, setValue] = useState(() => {
    return getLocalValue(key, initValue);
  });

   useEffect(() => {
     localStorage.setItem(key, JSON.stringify(value));
   }, [key, value])

   return [value, setValue];
}

export default useLocalStorage;