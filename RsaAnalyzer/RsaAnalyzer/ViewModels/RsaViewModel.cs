﻿using System.Windows.Input;
using RsaAnalyzer.Models;
using RsaAnalyzer.Utilities;

namespace RsaAnalyzer.ViewModels
{
    internal class RsaViewModel : BaseViewModel
    {
        private Rsa _rsa;

        private uint _n;
        private uint _e;
        private long _d;

        private ushort _plainByte;
        private long _encryptedByte;
        private int _decryptedByte;

        private volatile bool _initialized;
        private volatile bool _encrypting;
        private volatile bool _decrypting;

        public RsaViewModel()
        {
            _generatePrimes = new RelayCommand(p =>
            {
                if (!Initialized)
                {
                    PrepareRsa();

                    OnPropertyChanged(nameof(PublicKey));
                    OnPropertyChanged(nameof(PrivateKey));

                    Initialized = true;
                }

            }, p => !Initialized);

            _encryptByte = new RelayCommand(p =>
            {
                var result = new RsaProvider();

                if (!Encrypting)
                {
                    EncryptedByte = result.EncryptValue(PlainByte, E, N);

                    OnPropertyChanged(nameof(EncryptedByte));

                    Encrypting = true;
                }
                else
                {
                    Encrypting = false;
                }

                OnPropertyChanged(nameof(EncryptByte));
                OnPropertyChanged(nameof(DecryptByte));

            }, p => !Encrypting);

            _decryptByte = new RelayCommand(p =>
            {
                var result = new RsaProvider();

                if (!Decrypting)
                {
                    DecryptedByte = result.DecryptValue(EncryptedByte, D, N);

                    OnPropertyChanged(nameof(DecryptedByte));

                    Decrypting = true;
                }
                else
                {
                    Decrypting = false;
                }

            }, p => !Decrypting);
        }

        public bool Initialized
        {
            get => _initialized;
            set
            {
                _initialized = true;
                OnPropertyChanged();
            }
        }

        public bool Encrypting
        {
            get => _encrypting;
            set
            {
                _encrypting = value;
                OnPropertyChanged();

                GeneratePrimes.RaiseCanExecuteChanged();
                EncryptByte.RaiseCanExecuteChanged();
            }
        }

        public bool Decrypting
        {
            get => _decrypting;
            set
            {
                _decrypting = value;
                OnPropertyChanged();

                GeneratePrimes.RaiseCanExecuteChanged();
                DecryptByte.RaiseCanExecuteChanged();
            }
        }

        public Rsa Rsa
        {
            get => _rsa;
            set
            {
                _rsa = value;
                OnPropertyChanged();
            }
        }

        public uint N
        {
            get => _n;
            set
            {
                _n = value;
                OnPropertyChanged();
            }
        }

        public uint E
        {
            get => _e;
            set
            {
                _e = value;
                OnPropertyChanged();
            }
        }

        public long D
        {
            get => _d;
            set
            {
                _d = value;
                OnPropertyChanged();
            }
        }

        public ushort PlainByte
        {
            get => _plainByte;
            set
            {
                _plainByte = value;
                OnPropertyChanged();
            }
        }

        public long EncryptedByte
        {
            get => _encryptedByte;
            set
            {
                _encryptedByte = value;
                OnPropertyChanged();
            }
        }

        public int DecryptedByte
        {
            get => _decryptedByte;
            set
            {
                _decryptedByte = value;
                OnPropertyChanged();
            }
        }

        public string PublicKey => $"Pair: n = {N}, e = {E}";

        public string PrivateKey => $"Pair: d = {D}, n = {N}";

        public void PrepareRsa()
        {
            var values = RsaProvider.Run();
            N = values.Item1;
            E = values.Item2;
            D = values.Item3;

            Rsa = new Rsa(N, E, D);

            FileOperator.SaveRsaToFile("paniLodzia.txt", Rsa);
        }

        private RelayCommand _generatePrimes;
        private RelayCommand _encryptByte;
        private RelayCommand _decryptByte;

        public RelayCommand GeneratePrimes
        {
            get => _generatePrimes;
            set
            {
                _generatePrimes = value;
                OnPropertyChanged();
            }
            //{
            //    return _generatePrimes ?? (_generatePrimes = new RelayCommand(
            //               param =>
            //               {
            //                   PrepareRsa();

            //                   OnPropertyChanged(nameof(PublicKey));
            //                   OnPropertyChanged(nameof(PrivateKey));
            //               }
            //           ));
            //}
        }

        public RelayCommand EncryptByte
        {
            get => _encryptByte;
            set
            {
                _encryptByte = value;
                OnPropertyChanged();
            }
            //{
            //    var result = new RsaProvider();
            //    return _encryptByte ?? (_encryptByte = new RelayCommand(
            //               param =>
            //               {
            //                   EncryptedByte = result.EncryptValue(PlainByte, E, N);

            //                   OnPropertyChanged(nameof(EncryptedByte));
            //               }
            //           ));
            //}
        }

        public RelayCommand DecryptByte
        {
            get => _decryptByte;
            set
            {
                _decryptByte = value;
                OnPropertyChanged();
            }
            //{
            //    var result = new RsaProvider();
            //    return _decryptByte ?? (_decryptByte = new RelayCommand(
            //               param =>
            //               {
            //                   DecryptedByte = result.DecryptValue(EncryptedByte, D, N);

            //                   OnPropertyChanged(nameof(DecryptedByte));
            //               }
            //           ));
            //}
        }
    }
}
