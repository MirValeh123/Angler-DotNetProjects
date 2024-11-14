import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styles: ``,
})
export class LoginComponent implements OnInit {
  form!: FormGroup;
  isSubmitted:boolean = false;

  constructor(public formBuilder: FormBuilder) {}
  ngOnInit(): void {
    this.form = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  hasDisplayableError(controlName:string)
  {
    const control = this.form.get(controlName);
    return(
      Boolean(control?.valid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    )
  }

  onSubmit()
  {
    this.isSubmitted = true;
    if (this.form.valid) {
      console.log(this.form)
    }
  }
}
