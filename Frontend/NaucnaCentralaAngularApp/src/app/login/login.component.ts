import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  
  constructor(private httpClient: HttpClient, private router: Router, private toastrService: ToastrService) 
  { 
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      'username': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required)
    })
  }

  onSubmit(){
    this.httpClient.post('https://localhost:44372/api/user/login', this.loginForm.value, { responseType: 'text' }).subscribe(
      (res) => {
        localStorage.setItem('jwt', res.toString());

        this.httpClient.get('https://localhost:44372/api/user/role', { responseType: 'text' }).subscribe(
        (res1) => {
          localStorage.setItem('role', res1.toString())
          this.toastrService.success('Successfully logged in.', 'Success', { progressBar: true });
          this.router.navigate(['home']);
        },
        (err) => {
          this.toastrService.error('Could not log in.', 'Error', { progressBar: true });
        }
        )   
      },
      (error) => {
        this.toastrService.error('Could not log in.', 'Error', { progressBar: true });
      }
    )    
  }

}
