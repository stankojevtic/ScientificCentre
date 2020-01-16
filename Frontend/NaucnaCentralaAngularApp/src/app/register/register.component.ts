import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
   registerForm: FormGroup;
   scientificAreaList = ['Anatomy', 'Chemistry', 'Earth Science', 'Engineering'];

  constructor(private httpClient: HttpClient, private router: Router, private toastrService: ToastrService)  { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    let registerScientificAreas = new FormArray([]);
    registerScientificAreas.push(new FormGroup({
      'name': new FormControl(null, Validators.required)
    }));

    this.registerForm = new FormGroup({
      'username': new FormControl(null, Validators.required),
      'password': new FormControl(null, Validators.required),
      'firstname': new FormControl(null, Validators.required),
      'lastname': new FormControl(null, Validators.required),
      'city': new FormControl(null, Validators.required),
      'country': new FormControl(null, Validators.required),
      'vocation': new FormControl(),
      'isreviewer': new FormControl(false),
      'scientificAreas': registerScientificAreas
    })
  }

  onAddScientificArea() {
    (<FormArray>this.registerForm.get('scientificAreas')).push(
      new FormGroup({
        'name': new FormControl(null, Validators.required)
      })
    );
  }

  onSubmit()
  {
    this.httpClient.post('https://localhost:44372/api/user/register', this.registerForm.value).subscribe(
      (res) => {
        this.toastrService.success('Successfully submited data. Please verify email.', 'Success', { progressBar: true });
        this.router.navigate(['home']);
      },
      (error) => {
        this.toastrService.error('Could not register.', 'Error', { progressBar: true });
      }
    )
  }

}
