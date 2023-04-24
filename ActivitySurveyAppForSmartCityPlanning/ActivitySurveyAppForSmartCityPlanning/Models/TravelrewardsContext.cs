using System;
using System.Collections.Generic;
using ActivitySurveyAppForSmartCityPlanning.Models;
using Microsoft.EntityFrameworkCore;

namespace ActivitySurveyAppForSmartCityPlanning.Models;

public partial class TravelRewardsContext : DbContext
{
    public TravelRewardsContext()
    {
    }

    public TravelRewardsContext(DbContextOptions<TravelRewardsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountDetail> AccountDetails { get; set; }

    public virtual DbSet<AccountEmployment> AccountEmployments { get; set; }

    public virtual DbSet<AccountExtra> AccountExtras { get; set; }

    public virtual DbSet<AccountPointsTxn> AccountPointsTxns { get; set; }

    public virtual DbSet<AccountTriggeredSurvey> AccountTriggeredSurveys { get; set; }

    public virtual DbSet<GpsLog> GpsLogs { get; set; }

    public virtual DbSet<PublicTransport> PublicTransports { get; set; }

    public virtual DbSet<Response> Responses { get; set; }

    public virtual DbSet<ResponseQuestion> ResponseQuestions { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }

    public virtual DbSet<SurveyTrigger> SurveyTriggers { get; set; }

    public virtual DbSet<SurveyTriggerLog> SurveyTriggerLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccId);

            entity.ToTable("account");

            entity.Property(e => e.AccId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("acc_id");
            entity.Property(e => e.AccCurrentSession)
                .HasMaxLength(2047)
                .IsUnicode(false)
                .HasColumnName("acc_currentSession");
            entity.Property(e => e.AccDisable).HasColumnName("acc_disable");
            entity.Property(e => e.AccPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("acc_password");
            entity.Property(e => e.AccRole).HasColumnName("acc_role");
            entity.Property(e => e.AccUsername)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("acc_username");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
        });

        modelBuilder.Entity<AccountDetail>(entity =>
        {
            entity.HasKey(e => e.AccDetailsId).HasName("PK__account___1F009995C8273E23");

            entity.ToTable("account_details");

            entity.Property(e => e.AccDetailsId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("accDetails_id");
            entity.Property(e => e.AccDetailsAddressCity)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_addressCity");
            entity.Property(e => e.AccDetailsAddressCountry)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_addressCountry");
            entity.Property(e => e.AccDetailsAddressStreet)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_addressStreet");
            entity.Property(e => e.AccDetailsAddressZipCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_addressZipCode");
            entity.Property(e => e.AccDetailsAge).HasColumnName("accDetails_age");
            entity.Property(e => e.AccDetailsFirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_firstName");
            entity.Property(e => e.AccDetailsGender).HasColumnName("accDetails_gender");
            entity.Property(e => e.AccDetailsLastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_lastName");
            entity.Property(e => e.AccDetailsPhoneCountryCode)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_phoneCountryCode");
            entity.Property(e => e.AccDetailsPhoneNumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accDetails_phoneNumber");
            entity.Property(e => e.AccDetailsProfilePicture).HasColumnName("accDetails_profilePicture");
            entity.Property(e => e.AccDetailsTotalPoints).HasColumnName("accDetails_totalPoints");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");

            entity.HasOne(d => d.Acc).WithMany(p => p.AccountDetails)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__account_d__acc_i__26CFC035");
        });

        modelBuilder.Entity<AccountEmployment>(entity =>
        {
            entity.HasKey(e => e.AccEmpId).HasName("PK__account___CF95E86A3A9870AF");

            entity.ToTable("account_employment");

            entity.Property(e => e.AccEmpId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("accEmp_id");
            entity.Property(e => e.AccEmpAnnualSalary).HasColumnName("accEmp_annualSalary");
            entity.Property(e => e.AccEmpEndTime).HasColumnName("accEmp_endTime");
            entity.Property(e => e.AccEmpLocation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accEmp_location");
            entity.Property(e => e.AccEmpOccupation)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accEmp_occupation");
            entity.Property(e => e.AccEmpStartTime).HasColumnName("accEmp_startTime");
            entity.Property(e => e.AccEmpStatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accEmp_status");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");

            entity.HasOne(d => d.Acc).WithMany(p => p.AccountEmployments)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__account_e__acc_i__2AA05119");
        });

        modelBuilder.Entity<AccountExtra>(entity =>
        {
            entity.HasKey(e => e.AccExtraId).HasName("PK__account___AE127CC6CCFC3451");

            entity.ToTable("account_extra");

            entity.Property(e => e.AccExtraId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("accExtra_id");
            entity.Property(e => e.AccExtraDriverLicense)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accExtra_driverLicense");
            entity.Property(e => e.AccExtraHouseholdPosition)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("accExtra_householdPosition");
            entity.Property(e => e.AccExtraMobilityImpaired).HasColumnName("accExtra_mobilityImpaired");
            entity.Property(e => e.AccExtraNumberOfVehicles).HasColumnName("accExtra_numberOfVehicles");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");

            entity.HasOne(d => d.Acc).WithMany(p => p.AccountExtras)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__account_e__acc_i__2E70E1FD");
        });

        modelBuilder.Entity<AccountPointsTxn>(entity =>
        {
            entity.HasKey(e => e.AccPointsTxnId).HasName("PK__account___2D8D80012151AC04");

            entity.ToTable("account_points_txn");

            entity.Property(e => e.AccPointsTxnId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("accPointsTxn_id");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.AccPointsTxnAmt).HasColumnName("accPointsTxn_amt");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.RewardId).HasColumnName("reward_id");

            entity.HasOne(d => d.Acc).WithMany(p => p.AccountPointsTxns)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__account_p__acc_i__6CA31EA0");

            entity.HasOne(d => d.Reward).WithMany(p => p.AccountPointsTxns)
                .HasForeignKey(d => d.RewardId)
                .HasConstraintName("FK__account_p__rewar__15702A09");
        });

        modelBuilder.Entity<AccountTriggeredSurvey>(entity =>
        {
            entity.HasKey(e => e.AccTriggerId).HasName("PK__account___407F991B31F93D5A");

            entity.ToTable("account_triggered_survey");

            entity.Property(e => e.AccTriggerId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("accTrigger_id");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ExpireBy)
                .HasColumnType("datetime")
                .HasColumnName("expire_by");
            entity.Property(e => e.GpsId).HasColumnName("gps_id");
            entity.Property(e => e.GpsLogIds)
                .IsUnicode(false)
                .HasColumnName("gps_logIds");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");
            entity.Property(e => e.TriggerId).HasColumnName("trigger_id");

            entity.HasOne(d => d.Acc).WithMany(p => p.AccountTriggeredSurveys)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__account_t__acc_i__52793849");

            entity.HasOne(d => d.Gps).WithMany(p => p.AccountTriggeredSurveys)
                .HasForeignKey(d => d.GpsId)
                .HasConstraintName("FK__account_t__gps_i__041093DD");

            entity.HasOne(d => d.Survey).WithMany(p => p.AccountTriggeredSurveys)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__account_t__surve__51851410");

            entity.HasOne(d => d.Trigger).WithMany(p => p.AccountTriggeredSurveys)
                .HasForeignKey(d => d.TriggerId)
                .HasConstraintName("FK__account_t__trigg__536D5C82");
        });

        modelBuilder.Entity<GpsLog>(entity =>
        {
            entity.HasKey(e => e.GpsId).HasName("PK__gps_logs__9CE14CC527068D38");

            entity.ToTable("gps_logs");

            entity.Property(e => e.GpsId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("gps_id");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.Accuracy)
                .HasColumnType("decimal(20, 15)")
                .HasColumnName("accuracy");
            entity.Property(e => e.Altitude)
                .HasColumnType("decimal(21, 15)")
                .HasColumnName("altitude");
            entity.Property(e => e.AltitudeAccuracy)
                .HasColumnType("decimal(20, 15)")
                .HasColumnName("altitudeAccuracy");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.GpsTimestamp)
                .HasColumnType("datetime")
                .HasColumnName("gpsTimestamp");
            entity.Property(e => e.Heading)
                .HasColumnType("decimal(18, 15)")
                .HasColumnName("heading");
            entity.Property(e => e.Latitude)
                .HasColumnType("decimal(10, 7)")
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasColumnType("decimal(10, 7)")
                .HasColumnName("longitude");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.Speed)
                .HasColumnType("decimal(19, 16)")
                .HasColumnName("speed");

            entity.HasOne(d => d.Acc).WithMany(p => p.GpsLogs)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__gps_logs__acc_id__7E8CC4B1");
        });

        modelBuilder.Entity<PublicTransport>(entity =>
        {
            entity.HasKey(e => e.TransportId).HasName("PK__public_t__A17E327791BE2843");

            entity.ToTable("public_transport");

            entity.Property(e => e.TransportId)
                .ValueGeneratedNever()
                .HasColumnName("transport_id");
            entity.Property(e => e.TransportLatitude)
                .HasColumnType("decimal(10, 7)")
                .HasColumnName("transport_latitude");
            entity.Property(e => e.TransportLongitude)
                .HasColumnType("decimal(10, 7)")
                .HasColumnName("transport_longitude");
            entity.Property(e => e.TransportName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("transport_name");
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => e.ResId).HasName("PK__response__2090B50D36E7A7CA");

            entity.ToTable("response");

            entity.Property(e => e.ResId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("res_id");
            entity.Property(e => e.AccId).HasColumnName("acc_id");
            entity.Property(e => e.ByPublicTransport).HasColumnName("by_publicTransport");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.ResNoOfQns).HasColumnName("res_noOfQns");
            entity.Property(e => e.ResponseDisable).HasColumnName("response_disable");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");

            entity.HasOne(d => d.Acc).WithMany(p => p.Responses)
                .HasForeignKey(d => d.AccId)
                .HasConstraintName("FK__response__acc_id__75F77EB0");

            entity.HasOne(d => d.Survey).WithMany(p => p.Responses)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__response__survey__75035A77");
        });

        modelBuilder.Entity<ResponseQuestion>(entity =>
        {
            entity.HasKey(e => e.ResQnsId).HasName("PK__response__BE87F179398A04BA");

            entity.ToTable("response_question");

            entity.Property(e => e.ResQnsId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("resQns_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.QnsId).HasColumnName("qns_id");
            entity.Property(e => e.ResId).HasColumnName("res_id");
            entity.Property(e => e.ResQnsDecimal)
                .HasColumnType("decimal(20, 5)")
                .HasColumnName("resQns_decimal");
            entity.Property(e => e.ResQnsInt).HasColumnName("resQns_int");
            entity.Property(e => e.ResQnsString)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("resQns_string");
            entity.Property(e => e.ResQnsType).HasColumnName("resQns_type");

            entity.HasOne(d => d.Qns).WithMany(p => p.ResponseQuestions)
                .HasForeignKey(d => d.QnsId)
                .HasConstraintName("FK__response___qns_i__7ABC33CD");

            entity.HasOne(d => d.Res).WithMany(p => p.ResponseQuestions)
                .HasForeignKey(d => d.ResId)
                .HasConstraintName("FK__response___res_i__79C80F94");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__rewards__3DD599BC1E6D821D");

            entity.ToTable("rewards");

            entity.Property(e => e.RewardId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("reward_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.RewardDesc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("reward_desc");
            entity.Property(e => e.RewardImg).HasColumnName("reward_img");
            entity.Property(e => e.RewardName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("reward_name");
            entity.Property(e => e.RewardPoints).HasColumnName("reward_points");
            entity.Property(e => e.RewardQty).HasColumnName("reward_qty");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.SurveyId).HasName("PK__survey__9DC31A07B3446E92");

            entity.ToTable("survey");

            entity.Property(e => e.SurveyId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("survey_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.SurveyDesc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("survey_desc");
            entity.Property(e => e.SurveyDisable).HasColumnName("survey_disable");
            entity.Property(e => e.SurveyNoOfQns).HasColumnName("survey_noOfQns");
            entity.Property(e => e.SurveyPoints).HasColumnName("survey_points");
            entity.Property(e => e.SurveyTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("survey_title");
        });

        modelBuilder.Entity<SurveyQuestion>(entity =>
        {
            entity.HasKey(e => e.QnsId).HasName("PK__survey_q__E3DEA521BC30A55A");

            entity.ToTable("survey_question");

            entity.Property(e => e.QnsId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("qns_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.Qns)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns");
            entity.Property(e => e.QnsOption01)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option01");
            entity.Property(e => e.QnsOption02)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option02");
            entity.Property(e => e.QnsOption03)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option03");
            entity.Property(e => e.QnsOption04)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option04");
            entity.Property(e => e.QnsOption05)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option05");
            entity.Property(e => e.QnsOption06)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option06");
            entity.Property(e => e.QnsOption07)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option07");
            entity.Property(e => e.QnsOption08)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qns_option08");
            entity.Property(e => e.QnsOrder).HasColumnName("qns_order");
            entity.Property(e => e.QnsType).HasColumnName("qns_type");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyQuestions)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__survey_qu__surve__7132C993");
        });

        modelBuilder.Entity<SurveyTrigger>(entity =>
        {
            entity.HasKey(e => e.TriggerId).HasName("PK__survey_t__23E04327E2679693");

            entity.ToTable("survey_trigger");

            entity.Property(e => e.TriggerId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("trigger_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("deleted_by");
            entity.Property(e => e.LatLong)
                .IsUnicode(false)
                .HasColumnName("lat_long");
            entity.Property(e => e.ModifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("modified_at");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("modified_by");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");
            entity.Property(e => e.TriggerCooldown).HasColumnName("trigger_cooldown");
            entity.Property(e => e.TriggerRadius).HasColumnName("trigger_radius");

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveyTriggers)
                .HasForeignKey(d => d.SurveyId)
                .HasConstraintName("FK__survey_tr__surve__4DB4832C");
        });

        modelBuilder.Entity<SurveyTriggerLog>(entity =>
        {
            entity.HasKey(e => e.SurveyTriggerLogId).HasName("PK__survey_t__94E2FBBC00792BE9");

            entity.ToTable("survey_trigger_log");

            entity.Property(e => e.SurveyTriggerLogId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("survey_trigger_log_id");
            entity.Property(e => e.LogDateTime)
                .HasColumnType("datetime")
                .HasColumnName("log_date_time");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
