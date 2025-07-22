import { HttpInterceptorFn } from '@angular/common/http';

export const AuthInterceptorFn: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('authToken');
  const expiryTime = localStorage.getItem('expiryTime');

  // Auth URL’larda token qo‘shilmaydi
  if (req.url.includes('/User/Login') || req.url.includes('/User/Registration')) {
    return next(req);
  }

  // Token muddati tugagan bo‘lsa
  if (expiryTime && +expiryTime < Date.now()) {
    localStorage.removeItem('authToken');
    localStorage.removeItem('expiryTime');
    return next(req);
  }

  // Token mavjud bo‘lsa, headerga qo‘shiladi
  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(cloned);
  }

  return next(req);
};
