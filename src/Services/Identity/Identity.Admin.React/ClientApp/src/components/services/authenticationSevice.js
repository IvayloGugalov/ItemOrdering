const email = (val) =>{
  console.log(val);
  return val;
};

const refreshToken = '';

const authHeader = () => {
  if (email) {
    return { Authorization: `Bearer ${refreshToken}` };
  } else {
    return {};
  }
}

export const authenticationService = {
  currentUser: email,
  getAuthHeader: authHeader
  // currentUser: currentUserSubject.asObservable(),
  // get currentUserValue () { return currentUserSubject.value }
};