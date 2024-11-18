import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './dashboard.component.html',
  styles: ``,
})
export class DashboardComponent {
  constructor(private router: Router, public service: AuthService) {}

  onLogout() {
    this.service.deleteToken();
    this.router.navigateByUrl('/login');
  }
}
