import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-correct-magazine-data',
  templateUrl: './correct-magazine-data.component.html',
  styleUrls: ['./correct-magazine-data.component.scss']
})
export class CorrectMagazineDataComponent implements OnInit {
  magazineCorrectForm: FormGroup;
  id: number;
  magazine = null;
  scientificAreaList = ['Anatomy', 'Chemistry', 'Earth Science', 'Engineering'];

  constructor(private httpClient: HttpClient, private router: Router, private route: ActivatedRoute, private toastrService: ToastrService)  { }

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.initForm();
      }
    )
  }

  initForm() {
    let scientificAreas = new FormArray([]);
    scientificAreas.push(new FormGroup({
      'name': new FormControl(null, Validators.required)
    }));

    this.magazineCorrectForm = new FormGroup({
      'name': new FormControl(null, Validators.required),
      'issn': new FormControl(null, Validators.required),
      'isopenaccess': new FormControl(false),
      'scientificAreas': scientificAreas
    })
  }

  onAddScientificArea() {
    (<FormArray>this.magazineCorrectForm.get('scientificAreas')).push(
      new FormGroup({
        'name': new FormControl(null, Validators.required)
      })
    );
  }

  onSubmit() {
    debugger;
    this.httpClient.post(`https://localhost:44372/api/editortask/correct/${this.id}`, this.magazineCorrectForm.value).subscribe(
      (res) => {
        this.toastrService.success('Successfully corrected magazine data.', 'Success', { progressBar: true });
        this.router.navigate(['home']);
      },
      (err) => {
        this.toastrService.error('Could not correct magazine data.', 'Error', { progressBar: true });
      }
    )
  }

}
