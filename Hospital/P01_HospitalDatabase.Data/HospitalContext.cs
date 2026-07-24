namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options) { }

        public virtual DbSet<Patient> Patients
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Visitation> Visitations { get; set; } = null!;

        public virtual DbSet<Diagnose> Diagnoses { get; set; } = null!;

        public virtual DbSet<Medicament> Medicaments { get; set; } = null!;

        public virtual DbSet<PatientMedicament> Prescriptions
        {
            get;
            set;
        } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HospitalContext).Assembly);
        }
    }
}
