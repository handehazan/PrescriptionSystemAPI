﻿using MongoDB.Bson.IO;
using prescriptionSystemApi.model;
using prescriptionSystemApi.model.dto;
using prescriptionSystemApi.source.db;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace prescriptionSystemApi.source.svc
{
    public class PrescriptionService: IPrescriptionService
    {
        private readonly PrescriptionAccess _prescriptionAccess;
        private readonly RabbitmqService _rabbitmqService;
        public PrescriptionService(PrescriptionAccess prescriptionAccess,RabbitmqService rabbitmqService)
        {
            _prescriptionAccess = prescriptionAccess;
            _rabbitmqService = rabbitmqService;
        }
        public async Task<Prescription> CreatePrescriptionAsync(CreatePrescriptionDto dto)
        {
            var prescription = new Prescription()
            {
                PatientTC = dto.PatientTC,
                VisitDate = dto.VisitDate,
                DoctorId = dto.DoctorId,
                IsSubmitted = false,
            };
            await _prescriptionAccess.CreatePrescriptionAsync(prescription);

            foreach (var medicineDto in dto.Medicines)
            {
                var prescriptionMedicine = new PrescriptionMedicines()
                {
                    MedicineName = medicineDto.MedicineName,
                    PrescriptionID = prescription.PrescriptionId
                };
                await _prescriptionAccess.AddPrescriptionMedicineAsync(prescriptionMedicine);
            }
            return prescription;
        }

        public async Task<List<Prescription>> GetPrescriptionByPatientTCAsync(string patientTC)
        {
           var prescriptions= await _prescriptionAccess.GetPrescriptionByPatientTCAsync(patientTC);
            return prescriptions.Where(p => !p.IsSubmitted).ToList();

           
               
        }

        public async Task<List<PrescriptionMedicines>> GetMedicinesByPrescriptionIdAsync(int prescriptionId)
        {
            return await _prescriptionAccess.GetMedicinesByPrescriptionIdAsync(prescriptionId);
        }

        public async Task<SubmitPrescriptionResponseDto> SubmitPrescriptionAsync(SubmitPrescriptionDto dto)
        {
            var prescriptionMedicines = await _prescriptionAccess.GetMedicinesByPrescriptionIdAsync(dto.prescriptionId);
            if (prescriptionMedicines == null || !prescriptionMedicines.Any())
            {
                throw new Exception("Prescription not found or has no medicines");
            }
            var prescribedMedicineNames = prescriptionMedicines.Select(m => m.MedicineName).ToList();
             
            var missingMedicines = prescribedMedicineNames.Except(dto.MedicinesGiven,StringComparer.OrdinalIgnoreCase).ToList();

            if (missingMedicines.Any())
            {
                var message = new
                {
                    pharmacyName = dto.pharmacyName,
                    prescriptionId = dto.prescriptionId,
                    missingMedicines = missingMedicines,
                };
                //convert this message to the JSON
                var jsonMsg = JsonConvert.SerializeObject(message);

                _rabbitmqService.PublishMessage("missing-medicine",jsonMsg);
            }

            await _prescriptionAccess.MarkPrescriptionAsSubmittedAsync(dto.prescriptionId);

            return new SubmitPrescriptionResponseDto
            {
                PharmacyName = dto.pharmacyName,
                PrescriptionId = dto.prescriptionId,
                MissingMedicines = missingMedicines,
            };
        }
    }
}
