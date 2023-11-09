import { inject } from '@angular/core'; import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';

export const isAdminGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService)
  const router = inject(Router)
  let isAdmin = false;
  auth.isAdmin.subscribe((value) => {
    isAdmin = value;
  })
  if (isAdmin) {
    return true
  } else {
    router.navigate(['/login'])
    return false;
  }
};
