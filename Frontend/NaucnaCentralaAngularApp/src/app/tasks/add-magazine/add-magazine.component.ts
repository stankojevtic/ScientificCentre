import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-magazine',
  templateUrl: './add-magazine.component.html',
  styleUrls: ['./add-magazine.component.scss']
})
export class AddMagazineComponent implements OnInit {
  magazineCreateForm: FormGroup;
  scientificAreaList = ['Anatomy', 'Chemistry', 'Earth Science', 'Engineering'];
  
  constructor(private httpClient: HttpClient, private router: Router, private toastrService: ToastrService)  { }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    let scientificAreas = new FormArray([]);
    scientificAreas.push(new FormGroup({
      'name': new FormControl(null, Validators.required)
    }));

    this.magazineCreateForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'issn': new FormControl(null, Validators.required),
      'isopenaccess': new FormControl(false),
      'scientificAreas': scientificAreas
    })
  }

  onAddScientificArea() {
    (<FormArray>this.magazineCreateForm.get('scientificAreas')).push(
      new FormGroup({
        'name': new FormControl(null, Validators.required)
      })
    );
  }


  onSubmit()
  {
    this.httpClient.post('https://localhost:44372/api/magazine', this.magazineCreateForm.value).subscribe(
      (res) => {
        this.toastrService.success('Successfully created Magazine.', 'Success', { progressBar: true });
        this.router.navigate(['home']);
      },
      (error) => {
        this.toastrService.error('Could not create magazines.', 'Error', { progressBar: true });
        this.router.navigate(['home']);
      }
    )
  }


}